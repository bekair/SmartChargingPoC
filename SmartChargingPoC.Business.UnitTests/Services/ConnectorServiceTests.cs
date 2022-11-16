using System.Text.Json;
using AutoMapper;
using SmartChargingPoC.Business.Dtos.Requests.Connectors;
using SmartChargingPoC.Business.Dtos.Responses.Connectors;
using SmartChargingPoC.Business.Services;
using SmartChargingPoC.Business.Services.Interfaces;
using SmartChargingPoC.Core.Constants;
using SmartChargingPoC.Core.Entities;
using SmartChargingPoC.DataAccess.Exceptions;
using SmartChargingPoC.DataAccess.UnitOfWorks.Base;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using SmartChargingPoC.Business.UnitTests.Mocks;

namespace SmartChargingPoC.Business.UnitTests.Services;

[TestFixture]
public class ConnectorServiceTests
{
    private IConnectorService _connectorService;
    private IGroupService _mockGroupService;
    private IUnitOfWork _mockUnitOfWork;
    private IMapper _mockMapper;
    
    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = Substitute.For<IUnitOfWork>();
        _mockMapper = Substitute.For<IMapper>();
        _mockGroupService = Substitute.For<IGroupService>();
        
        _connectorService = new ConnectorService(
            _mockUnitOfWork,
            _mockMapper,
            _mockGroupService
        );
    }
    
    [Test]
    public void GivenConnectorId_WhenCallGetConnectorByAnyId_ThenReturnsSuccessfulConnectorResponseDto()
    {
        //Arrange
        const int connectorId = 1;
        var getConnectorRepoResult = MockConnectorData.ConnectorData(connectorId);
        var result = MockConnectorData.MapConnectorToConnectorResponseDtoData(getConnectorRepoResult);

        _mockUnitOfWork.ConnectorRepository
            .GetConnector(connectorId)
            .Returns(getConnectorRepoResult);

        _mockMapper
            .Map<ConnectorResponseDto>(getConnectorRepoResult)
            .Returns(result);

        //Act
        var connectorResponseDto = _connectorService.GetConnector(connectorId);

        //Assert
        Assert.That(
            JsonSerializer.Serialize(connectorResponseDto),
            Is.EqualTo(JsonSerializer.Serialize(result))
        );
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GivenConnectorIdLessThanOrEqualToZero_WhenCallGetConnector_ThenThrowsDataNotFoundException(int connectorId)
    {
        //Arrange
        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.Connector);
        var expectedException = new DataNotFoundException(exceptionMessage);
        _mockUnitOfWork.ConnectorRepository
            .GetConnector(connectorId)
            .Throws(expectedException);

        //Act
        var getConnectorDelegate = new TestDelegate(
            () => _connectorService.GetConnector(connectorId)
        );

        //Assert
        var exception = Assert.Throws<DataNotFoundException>(getConnectorDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(DataNotFoundException)));
        });
    }
    
    [Test]
    public void WhenCallGetAllConnectorsOnNoDataOnConnectorTable_ThenReturnsEmptyList()
    {
        //Arrange
        var emptyGetAllConnectorResult = Array.Empty<Connector>();
        var mappedResult = Array.Empty<ConnectorResponseDto>();
        
        _mockUnitOfWork.ConnectorRepository
            .Get()
            .Returns(emptyGetAllConnectorResult);

        _mockMapper
            .Map<ICollection<ConnectorResponseDto>>(emptyGetAllConnectorResult)
            .Returns(mappedResult);

        //Act
        var connectorResponseDtoList = _connectorService.GetAllConnectors();

        //Assert
        Assert.That(connectorResponseDtoList, Is.Empty);
    }

    [Test]
    public void WhenCallGetAllConnectors_ThenReturnsAllConnectorsAsConnectorResponseDtoCollection()
    {
        //Arrange
        var connectorIdArray = new[] { 1, 2, 3 };
        var connectorCollection = MockConnectorData.ConnectorCollection(connectorIdArray);
        var connectorResponseDtoCollection = MockConnectorData.ConnectorResponseDtoCollection(connectorIdArray);
        
        _mockUnitOfWork.ConnectorRepository
            .Get()
            .Returns(connectorCollection);
        
        _mockMapper
            .Map<ICollection<ConnectorResponseDto>>(connectorCollection)
            .Returns(connectorResponseDtoCollection);
        
        //Act
        var connectorResponseDtoResultList = _connectorService.GetAllConnectors();
        
        //Assert
        Assert.That(connectorResponseDtoResultList, Is.Not.Empty);
        Assert.That(
            JsonSerializer.Serialize(connectorResponseDtoResultList),
            Is.EqualTo(JsonSerializer.Serialize(connectorResponseDtoCollection))
        );
    }
    
    [Test]
    public void GivenCreateConnectorRequestDto_WhenCallCreateConnector_ThenReturnsCreateConnectorResponseDto()
    {
        //Arrange
        const int connectorId = 1;
        const int lastChargeStationUniqueNumber = 2;
        const int newChargeStationUniqueNumber = lastChargeStationUniqueNumber + 1;
        const int groupId = 1;
        const int chargeStationId = 1;
        const double maxCurrentInAmps = 100;
        
        var connectorData = MockConnectorData.ConnectorData(connectorId);
        var createConnectorRequestDto = MockConnectorData.CreateConnectorRequestDtoData(chargeStationId, maxCurrentInAmps);
        var connectorResponseDtoData = MockConnectorData.CreateConnectorResponseDtoData(connectorId, newChargeStationUniqueNumber, createConnectorRequestDto);

        _mockUnitOfWork.GroupRepository
            .GetGroupIdByChargeStationId(createConnectorRequestDto.ChargeStationId)
            .Returns(groupId);

        _mockUnitOfWork.ConnectorRepository
            .GetLastConnectorUniqueNumber(createConnectorRequestDto.ChargeStationId)
            .Returns(lastChargeStationUniqueNumber);
        
        _mockMapper
            .Map<Connector>(createConnectorRequestDto)
            .Returns(connectorData);

        _mockMapper
            .Map<ConnectorResponseDto>(connectorData)
            .Returns(connectorResponseDtoData);

        //Act
        var createConnectorResponseDtoResult = _connectorService.CreateConnector(createConnectorRequestDto);

        //Assert
        _mockUnitOfWork.ChargeStationRepository.Received().CheckExistedChargeStation(createConnectorRequestDto.ChargeStationId, ApiConstants.ErrorMessage.NotExistedChargeStationInConnectorOperation);
        _mockUnitOfWork.ChargeStationRepository.Received().CheckConnectorSlotAvailability(createConnectorRequestDto.ChargeStationId);
        _mockGroupService.Received().CheckMaxCurrentCapacityExcess(groupId, createConnectorRequestDto.MaxCurrentInAmps);
        _mockUnitOfWork.Received().Commit();
        
        Assert.That(createConnectorResponseDtoResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createConnectorResponseDtoResult.Id, Is.EqualTo(connectorId));
            Assert.That(createConnectorResponseDtoResult.ChargeStationId, Is.EqualTo(createConnectorRequestDto.ChargeStationId));
            Assert.That(createConnectorResponseDtoResult.MaxCurrentInAmps, Is.EqualTo(createConnectorRequestDto.MaxCurrentInAmps));
            Assert.That(createConnectorResponseDtoResult.ChargeStationUniqueNumber, Is.EqualTo(newChargeStationUniqueNumber));
        });
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GivenCreateConnectorRequestDtoWithMaxCurrentInAmpsLessThanOrEqualToZero_WhenCallCreateConnector_ThenThrowsLessThanValueException(int maxCurrentInAmps)
    {
        //Arrange
        const int chargeStationId = 1;

        var createConnectorRequestDto = MockConnectorData.CreateConnectorRequestDtoData(chargeStationId, maxCurrentInAmps);
    
        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.GreaterThan, ApiConstants.PropertyName.MaxCurrentInAmps, ApiConstants.General.MinCapacityInAmpsValue);
        var expectedException = new LessThanValueException(exceptionMessage);
    
        //Act
        var createConnectionDelegate = new TestDelegate(
            () => _connectorService
                .CreateConnector(createConnectorRequestDto)
                .Throws(expectedException)
        );
    
        //Assert
        var exception = Assert.Throws<LessThanValueException>(createConnectionDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(LessThanValueException)));
        });
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GivenCreateConnectorRequestDtoWithNotExistedChargeStationId_WhenCallCreateConnector_ThenThrowsDataNotFoundException(int chargeStationId)
    {
        //Arrange
        const double maxCurrentInAmps = 100;
        
        var createConnectorRequestDto = MockConnectorData.CreateConnectorRequestDtoData(chargeStationId, maxCurrentInAmps);

        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.ChargeStation);
        var expectedException = new DataNotFoundException(exceptionMessage);
        
        _mockUnitOfWork.ChargeStationRepository
            .When(c=> c.CheckExistedChargeStation(createConnectorRequestDto.ChargeStationId, ApiConstants.ErrorMessage.NotExistedChargeStationInConnectorOperation))
            .Do(c=> throw expectedException);

        //Act
        var createConnectorDelegate = new TestDelegate(
            () => _connectorService
                .CreateConnector(createConnectorRequestDto)
        );
    
        //Assert
        var exception = Assert.Throws<DataNotFoundException>(createConnectorDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(DataNotFoundException)));
        });
    }
    
    [Test]
    public void GivenAnyCreateConnectorRequestDto_NotAvailableSlot_WhenCallCreateConnector_ThenThrowsDataNotFoundException()
    {
        //Arrange
        const double maxCurrentInAmps = 100;
        const int chargeStationId = 2;
        const string exceptionMessage = ApiConstants.ErrorMessage.NoConnectorSlotInChargeStation;
        
        var createConnectorRequestDto = MockConnectorData.CreateConnectorRequestDtoData(chargeStationId, maxCurrentInAmps);
        var expectedException = new DataNotFoundException(exceptionMessage);
        
        _mockUnitOfWork.ChargeStationRepository
            .When(c=> c.CheckConnectorSlotAvailability(Arg.Any<int>()))
            .Do(c=> throw expectedException);

        //Act
        var createConnectorDelegate = new TestDelegate(
            () => _connectorService
                .CreateConnector(createConnectorRequestDto)
        );
    
        //Assert
        var exception = Assert.Throws<DataNotFoundException>(createConnectorDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(DataNotFoundException)));
        });
    }
    
    [Test]
    public void GivenCreateConnectorRequestDtoAsNull_WhenCallCreateConnector_ThenThrowsArgumentNullException()
    {
        //Arrange
        CreateConnectorRequestDto createConnectorRequestDto = null;
        var exceptionMessage = $"{string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.General.RequestedParameter)} (Parameter '{nameof(createConnectorRequestDto)}')";
        var expectedException = new ArgumentNullException(
            string.Empty,
            exceptionMessage
        );
    
        //Act
        var createConnectorDelegate = new TestDelegate(
            () => _connectorService
                .CreateConnector(createConnectorRequestDto)
        );
    
        //Assert
        var exception = Assert.Throws<ArgumentNullException>(createConnectorDelegate);
    
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ArgumentNullException)));
        });
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GivenIdAsLessThanOrEqualToZeroAndAnyUpdateConnectorRequestDto_WhenCallUpdateConnector_ThenThrowsDataNotFoundException(int connectorId)
    {
        //Arrange
        var updateConnectorRequestDto = new UpdateConnectorRequestDto();
    
        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.DataNotFound, nameof(ApiConstants.PropertyName.Connector));
        var expectedException = new DataNotFoundException(exceptionMessage);

        _mockUnitOfWork.ConnectorRepository
            .GetConnector(connectorId)
            .Throws(expectedException);
    
        //Act
        var updateConnectionDelegate = new TestDelegate(
            () => _connectorService
                .UpdateConnector(connectorId, updateConnectorRequestDto)
        );
    
        //Assert
        var exception = Assert.Throws<DataNotFoundException>(updateConnectionDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(DataNotFoundException)));
        });
    }
    
    [Test]
    public void GivenAnyIdAndAnyUpdateConnectorRequestDto_GroupIdNotFoundWithConnectorId_WhenCallUpdateConnector_ThenThrowsArgumentNullException()
    {
        //Arrange
        const int connectorId = 1;
        const string exceptionMessage = $"{ApiConstants.ErrorMessage.NotExistedGroupIdByConnectorId} (Parameter 'groupId')";
        
        var updateConnectorRequestDto = new UpdateConnectorRequestDto();
    
        var expectedException = new ArgumentNullException(
            string.Empty,
            exceptionMessage
        );

        _mockUnitOfWork.GroupRepository
            .GetGroupIdByConnectorId(connectorId)
            .Throws(expectedException);
    
        //Act
        var updateConnectionDelegate = new TestDelegate(
            () => _connectorService
                .UpdateConnector(connectorId, updateConnectorRequestDto)
        );
    
        //Assert
        var exception = Assert.Throws<ArgumentNullException>(updateConnectionDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ArgumentNullException)));
        });
    }
    
    [Test]
    public void GivenAnyIdAndAnyUpdateConnectorRequestDto_MaxCurrentCapacityExceeded_WhenCallUpdateConnector_ThenThrowsExceededCurrentCapacityException()
    {
        //Arrange
        const int connectorId = 1;
        const string exceptionMessage = ApiConstants.ErrorMessage.ExceededCurrentCapacity;
        
        var anyUpdateConnectorRequestDto = new UpdateConnectorRequestDto();
        var anyConnector = new Connector();
    
        var expectedException = new ExceededCurrentCapacityException(exceptionMessage);

        _mockUnitOfWork.ConnectorRepository
            .GetConnector(connectorId)
            .Returns(anyConnector);

        _mockGroupService
            .When(s=>s.CheckMaxCurrentCapacityExcess(Arg.Any<int>(), Arg.Any<double>()))
            .Do(call => throw expectedException);
    
        //Act
        var updateConnectionDelegate = new TestDelegate(
            () => _connectorService
                .UpdateConnector(connectorId, anyUpdateConnectorRequestDto)
        );
    
        //Assert
        var exception = Assert.Throws<ExceededCurrentCapacityException>(updateConnectionDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ExceededCurrentCapacityException)));
        });
    }
    
    [Test]
    public void GivenAnyValidIdAndAnyValidUpdateConnectorRequestDto_WhenCallUpdateConnector_ThenAssertAsSuccessful()
    {
        //Arrange
        const int groupId = 1;
        const int connectorId = 1;
        const double maxCurrentInAmps = 100;
        
        var connectorData = MockConnectorData.ConnectorData(connectorId);
        var updateConnectorRequestDto = MockConnectorData.UpdateConnectorRequestDtoData(maxCurrentInAmps);

        _mockUnitOfWork.ConnectorRepository
            .GetConnector(connectorId)
            .Returns(connectorData);

        _mockUnitOfWork.GroupRepository
            .GetGroupIdByConnectorId(connectorId)
            .Returns(groupId);

        //Act
        _connectorService.UpdateConnector(connectorId, updateConnectorRequestDto);

        //Assert
        _mockUnitOfWork.ConnectorRepository.Received().UpdateConnector(connectorData);
        _mockUnitOfWork.Received().Commit();
        
        Assert.That(true);
    }
    
    [Test]
    public void GivenAnyGreaterThanZeroId_WhenCallDeleteConnector_ThenExpectsDeletedAssertTrue()
    {
        //Arrange
        const int anyConnectorId = 1;
        
        //Act
        _connectorService.DeleteConnector(anyConnectorId);
        
        //Assert
        _mockUnitOfWork.ConnectorRepository.Received().DeleteConnector(anyConnectorId);
        _mockUnitOfWork.Received().Commit();

        Assert.That(true);
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GivenIdLessThanOrEqualToZero_WhenCallDeleteConnector_ThenThrowsDataNotFoundException(int connectorId)
    {
        //Arrange
        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.DataNotFound, nameof(ApiConstants.PropertyName.Connector));
        var expectedException = new DataNotFoundException(exceptionMessage);
        
        _mockUnitOfWork.ConnectorRepository
            .When(s=>s.DeleteConnector(connectorId))
            .Do(call => throw expectedException);
        
        //Act
        var deleteConnectorDelegate = new TestDelegate(
            () => _connectorService.DeleteConnector(connectorId)
        );
        
        //Assert
        _mockUnitOfWork.DidNotReceive().Commit();
        
        var exception = Assert.Throws<DataNotFoundException>(deleteConnectorDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(DataNotFoundException)));
        });
    }
}