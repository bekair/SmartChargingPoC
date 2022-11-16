using System.Text.Json;
using AutoMapper;
using SmartChargingPoC.Business.Dtos.Requests.Groups;
using SmartChargingPoC.Business.Dtos.Responses.Groups;
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
public class GroupServiceTests
{
    private IGroupService _groupService;
    private IUnitOfWork _mockUnitOfWork;
    private IMapper _mockMapper;

    [SetUp]
    public void Setup()
    {
        _mockUnitOfWork = Substitute.For<IUnitOfWork>();
        _mockMapper = Substitute.For<IMapper>();

        _groupService = new GroupService(
            _mockUnitOfWork,
            _mockMapper
        );
    }

    [Test]
    public void GivenGroupId_WhenCallGetGroupByAnyId_ThenReturnsSuccessfulGroupResponseDto()
    {
        //Arrange
        const int groupId = 1;

        _mockUnitOfWork.GroupRepository
            .GetGroup(groupId)
            .Returns(MockGroupData.GroupData(groupId));

        _mockMapper
            .Map<GroupResponseDto>(Arg.Any<Group>())
            .Returns(MockGroupData.GroupResponseDtoData(groupId));

        //Act
        var groupResponseDto = _groupService.GetGroup(groupId);

        //Assert
        Assert.That(
            JsonSerializer.Serialize(groupResponseDto),
            Is.EqualTo(JsonSerializer.Serialize(MockGroupData.GroupResponseDtoData(groupId)))
        );
    }

    [Test]
    [TestCase(0)]
    public void GivenGroupId_WhenCallGetGroup_ThenThrowsDataNotFoundException(int groupId)
    {
        //Arrange
        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.Group);
        var expectedException = new DataNotFoundException(exceptionMessage);
        _mockUnitOfWork.GroupRepository
            .GetGroup(groupId)
            .Throws(expectedException);

        //Act
        var getGroupDelegate = new TestDelegate(
            () => _groupService.GetGroup(groupId)
        );

        //Assert
        var exception = Assert.Throws<DataNotFoundException>(getGroupDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(DataNotFoundException)));
        });
    }

    [Test]
    public void WhenCallGetAllGroupsOnNoDataOnGroupTable_ThenReturnsEmptyList()
    {
        //Arrange
        _mockUnitOfWork.GroupRepository
            .GetAllGroups()
            .Returns(Array.Empty<Group>());

        _mockMapper
            .Map<ICollection<GroupResponseDto>>(Arg.Any<ICollection<Group>>())
            .Returns(Array.Empty<GroupResponseDto>());

        //Act
        var groupResponseDtoList = _groupService.GetAllGroups();

        //Assert
        Assert.That(groupResponseDtoList, Is.Empty);
    }

    [Test]
    public void WhenCallGetAllGroups_ThenReturnsAllGroupsAsGroupResponseDtoCollection()
    {
        //Arrange
        var groupIdArray = new[] { 1, 2, 3 };
        var groupCollection = MockGroupData.GroupCollection(groupIdArray);
        var groupResponseDtoCollection = MockGroupData.GroupResponseDtoCollection(groupIdArray);

        _mockUnitOfWork.GroupRepository
            .GetAllGroups()
            .Returns(groupCollection);

        _mockMapper
            .Map<ICollection<GroupResponseDto>>(Arg.Any<ICollection<Group>>())
            .Returns(groupResponseDtoCollection);

        //Act
        var groupResponseDtoResultList = _groupService.GetAllGroups();

        //Assert
        Assert.That(groupResponseDtoResultList, Is.Not.Empty);
        Assert.That(
            JsonSerializer.Serialize(groupResponseDtoResultList),
            Is.EqualTo(JsonSerializer.Serialize(groupResponseDtoCollection))
        );
    }

    [Test]
    [TestCase(1)]
    public void GivenCreateGroupRequestDto_WhenCallCreateGroup_ThenReturnsCreateGroupResponseDto(int groupId)
    {
        //Arrange
        var groupData = MockGroupData.GroupData(groupId);
        var createGroupRequestDto = MockGroupData.CreateGroupRequestDtoData("GROUP 1", 100);
        var createGroupResponseDtoData = MockGroupData.CreateGroupResponseDtoData(groupId, createGroupRequestDto);

        _mockMapper
            .Map<Group>(createGroupRequestDto)
            .Returns(groupData);

        _mockMapper
            .Map<CreateGroupResponseDto>(groupData)
            .Returns(createGroupResponseDtoData);

        //Act
        var createGroupResponseDtoResult = _groupService.CreateGroup(createGroupRequestDto);

        //Assert
        Assert.That(createGroupResponseDtoResult, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(createGroupResponseDtoResult.Id, Is.EqualTo(groupId));
            Assert.That(createGroupResponseDtoResult.Name, Is.EqualTo(createGroupRequestDto.Name));
            Assert.That(createGroupResponseDtoResult.CapacityInAmps, Is.EqualTo(createGroupRequestDto.CapacityInAmps));
        });
    }

    [Test]
    public void GivenCreateGroupRequestDtoWithNameAsNull_WhenCallCreateGroup_ThenThrowsArgumentNullException()
    {
        //Arrange
        var createGroupRequestDto = MockGroupData.CreateGroupRequestDtoData(null, 100);

        var exceptionMessage = string.Format($"{ApiConstants.ErrorMessage.Required} (Parameter '{ApiConstants.PropertyName.Name}')", ApiConstants.PropertyName.Name);
        var expectedException = new ArgumentNullException(
            string.Empty,
            exceptionMessage
        );

        //Act
        var createGroupDelegate = new TestDelegate(
            () => _groupService
                .CreateGroup(createGroupRequestDto)
                .Throws(expectedException)
        );

        //Assert
        var exception = Assert.Throws<ArgumentNullException>(createGroupDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ArgumentNullException)));
        });
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    [TestCase(-2)]
    public void GivenCreateGroupRequestDtoWithCapacityInAmpsIsLessThanOrEqualToZero_WhenCallCreateGroup_ThenThrowsLessThanValueException(double capacityImAmps)
    {
        //Arrange
        var createGroupRequestDto = MockGroupData.CreateGroupRequestDtoData("Name 1", capacityImAmps);

        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.GreaterThan,
            ApiConstants.PropertyName.CapacityInAmps, ApiConstants.General.MinCapacityInAmpsValue);
        var expectedException = new LessThanValueException(exceptionMessage);

        //Act
        var createGroupDelegate = new TestDelegate(
            () => _groupService
                .CreateGroup(createGroupRequestDto)
                .Throws(expectedException)
        );

        //Assert
        var exception = Assert.Throws<LessThanValueException>(createGroupDelegate);
        
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(LessThanValueException)));
        });
    }

    [Test]
    public void GivenCreateGroupRequestDtoAsNull_WhenCallCreateGroup_ThenThrowsArgumentNullException()
    {
        //Arrange
        CreateGroupRequestDto createGroupRequestDto = null;
        var exceptionMessage = $"{string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.General.RequestedParameter)} (Parameter '{nameof(createGroupRequestDto)}')";
        var expectedException = new ArgumentNullException(
            string.Empty,
            exceptionMessage
        );

        //Act
        var createGroupDelegate = new TestDelegate(
            () => _groupService
                .CreateGroup(createGroupRequestDto)
                .Throws(expectedException)
        );

        //Assert
        _mockMapper.DidNotReceive().Map<Group>(null);
        _mockUnitOfWork.GroupRepository.DidNotReceive().CreateGroup(Arg.Any<Group>());
        _mockUnitOfWork.DidNotReceive().Commit();

        var exception = Assert.Throws<ArgumentNullException>(createGroupDelegate);

        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ArgumentNullException)));
        });
    }

    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GivenIdLessThanOrEqualToZeroAndUpdateGroupRequestDtoAny_WhenCallUpdateGroup_ThenThrowsDataNotFoundException(int groupId)
    {
        //Arrange
        var anyUpdateGroupRequestDto = new UpdateGroupRequestDto
        {
            Name = "Name",
            CapacityInAmps = 1
        };
        var exceptionMessage = $"{string.Format(ApiConstants.ErrorMessage.DataNotFound, ApiConstants.PropertyName.Group)} (Parameter 'group')";
        var expectedException = new DataNotFoundException(exceptionMessage);
        
        _mockUnitOfWork.GroupRepository
            .GetGroup(groupId)
            .Throws(expectedException);

        //Act
        var updateGroupDelegate = new TestDelegate(
            () => _groupService.UpdateGroup(groupId, anyUpdateGroupRequestDto)
        );
        
        //Assert
        var exception = Assert.Throws<DataNotFoundException>(updateGroupDelegate);

        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(DataNotFoundException)));
        });
    }
    
    [Test]
    public void GivenAnyIdAndUpdateGroupRequestDtoAsNull_WhenCallUpdateGroup_ThenThrowsArgumentNullException()
    {
        //Arrange
        const int anyGroupId = 1;
        UpdateGroupRequestDto updateGroupRequestDto = null;
        var exceptionMessage = $"{string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.General.RequestedParameter)} (Parameter '{nameof(updateGroupRequestDto)}')";

        //Act
        var updateGroupDelegate = new TestDelegate(
            () => _groupService.UpdateGroup(anyGroupId, updateGroupRequestDto)
        );
        
        //Assert
        var exception = Assert.Throws<ArgumentNullException>(updateGroupDelegate);

        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(exceptionMessage));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ArgumentNullException)));
        });
    }
    
    [Test]
    public void GivenAnyIdAndUpdateGroupRequestDtoNameAsNull_WhenCallUpdateGroup_ThenThrowsArgumentNullException()
    {
        //Arrange
        const int anyGroupId = 1;
        var updateGroupRequestDto = new UpdateGroupRequestDto
        {
            Name = null,
            CapacityInAmps = 1
        };
        var exceptionMessage = $"{string.Format(ApiConstants.ErrorMessage.Required, ApiConstants.PropertyName.Name)} (Parameter '{nameof(UpdateGroupRequestDto.Name)}')";
        var expectedException = new ArgumentNullException(
            string.Empty,
            exceptionMessage
        );

        //Act
        var updateGroupDelegate = new TestDelegate(
            () => _groupService.UpdateGroup(anyGroupId, updateGroupRequestDto)
        );
        
        //Assert
        var exception = Assert.Throws<ArgumentNullException>(updateGroupDelegate);

        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ArgumentNullException)));
        });
    }
    
    [Test]
    [TestCase(0)]
    [TestCase(-1)]
    public void GivenAnyIdAndUpdateGroupRequestDtoCapacityInAmpsLessThanOrEqualToZero_WhenCallUpdateGroup_ThenThrowsLessThanValueException(double capacityInAmps)
    {
        //Arrange
        const int anyGroupId = 1;
        var updateGroupRequestDto = new UpdateGroupRequestDto
        {
            Name = "Any Name",
            CapacityInAmps = capacityInAmps
        };
        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.GreaterThan, ApiConstants.PropertyName.CapacityInAmps, ApiConstants.General.MinCapacityInAmpsValue);
        var expectedException = new LessThanValueException(exceptionMessage);

        //Act
        var updateGroupDelegate = new TestDelegate(
            () => _groupService.UpdateGroup(anyGroupId, updateGroupRequestDto)
        );
        
        //Assert
        var exception = Assert.Throws<LessThanValueException>(updateGroupDelegate);

        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(LessThanValueException)));
        });
    }
    
    [Test]
    [TestCase(1)]
    public void GivenAnyIdUpdateGroupRequestDtoAsValid_WhenCallCreateGroup_ThenAssertTrue(int groupId)
    {
        //Arrange
        var updatedGroupData = MockGroupData.GroupData(groupId);
        var updateGroupRequestDto = MockGroupData.UpdateGroupRequestDtoData("GROUP 1", 100);
        
        _mockUnitOfWork.GroupRepository
            .GetGroup(groupId)
            .Returns(updatedGroupData);

        //Act
        _groupService.UpdateGroup(groupId, updateGroupRequestDto);

        //Assert
        _mockMapper.Received().Map(updateGroupRequestDto, updatedGroupData);
        _mockUnitOfWork.GroupRepository.Received().UpdateGroup(updatedGroupData);
        _mockUnitOfWork.Received().Commit();
        
        Assert.That(true);
    }

    [Test]
    public void GivenIdLessThanOrEqualToZero_WhenCallDeleteGroup_ThenAssertTrue()
    {
        //Arrange
        const int anyGroupId = 1;
        
        //Act
        _groupService.DeleteGroup(anyGroupId);
        
        //Assert
        _mockUnitOfWork.GroupRepository.Received().DeleteGroup(anyGroupId);
        _mockUnitOfWork.Received().Commit();

        Assert.That(true);
    }
    
    [Test]
    [TestCase(1, 10)]
    [TestCase(2, 15)]
    public void GivenGroupIdAsGreaterThanZeroAndAnyNewMaxCurrentAdded_WhenCallCheckMaxCurrentCapacityExcess_ThenIsAvailableAssertTrue(int groupId, double newMaxCurrentCapacity)
    {
        //Arrange
        // DataNotFoundException(string.Format(ApiConstants.ErrorMessage.DataNotFound, typeof(TEntity).Name)
        const bool isCapacityAvailable = true;
        const double randomTotalMaxCurrentOfConnectors = 100.5;
        
        _mockUnitOfWork.ConnectorRepository
            .GetTotalMaxCurrentByGroupById(groupId)
            .Returns(randomTotalMaxCurrentOfConnectors);
        
        _mockUnitOfWork.GroupRepository
            .IsMaxCurrentCapacityAvailable(groupId, randomTotalMaxCurrentOfConnectors + newMaxCurrentCapacity)
            .Returns(isCapacityAvailable);
        
        //Act
        _groupService.CheckMaxCurrentCapacityExcess(groupId, newMaxCurrentCapacity);
        
        //Assert
        Assert.That(true);
    }
    
    [Test]
    [TestCase(1, 10)]
    [TestCase(2, 15)]
    public void GivenGroupIdAsGreaterThanZeroAndAnyNewMaxCurrentAdded_WhenCallCheckMaxCurrentCapacityExcess_ThenThrowsExceededCurrentCapacityException(int groupId, double newMaxCurrentCapacity)
    {
        //Arrange
        // DataNotFoundException(string.Format(ApiConstants.ErrorMessage.DataNotFound, typeof(TEntity).Name)
        const bool isCapacityAvailable = false;
        const double randomTotalMaxCurrentOfConnectors = 100.5;
        var exceptionMessage = string.Format(ApiConstants.ErrorMessage.ExceededCurrentCapacity);
        var expectedException = new ExceededCurrentCapacityException(exceptionMessage);
        
        _mockUnitOfWork.ConnectorRepository
            .GetTotalMaxCurrentByGroupById(groupId)
            .Returns(randomTotalMaxCurrentOfConnectors);
        
        _mockUnitOfWork.GroupRepository
            .IsMaxCurrentCapacityAvailable(groupId, randomTotalMaxCurrentOfConnectors + newMaxCurrentCapacity)
            .Returns(isCapacityAvailable);
        
        //Act
        var checkMaxCurrentCapacityExcessDelegate = new TestDelegate(
            () => _groupService.CheckMaxCurrentCapacityExcess(groupId, newMaxCurrentCapacity)
        );
        
        //Assert
        var exception = Assert.Throws<ExceededCurrentCapacityException>(checkMaxCurrentCapacityExcessDelegate);
        Assert.Multiple(() =>
        {
            Assert.That(exception?.Message, Is.EqualTo(expectedException.Message));
            Assert.That(exception?.GetType(), Is.EqualTo(typeof(ExceededCurrentCapacityException)));
        });
    }
}