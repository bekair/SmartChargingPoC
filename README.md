# GreenFluxSmartCharging

* RepositoryBase should be enough for this work but I wanted to add an extra layer for every context
  implementation. You can select your context to send it into RepositoryBase and extend your specific 
  RepositoryBase with any DbContext implementation like in SmartChargingRepositoryBase.
* Hard delete is selected instead of soft delete. I preferred to use hard
  delete.
* I didn't add any UpdateDate and CreatedDate fields for the entities. If I want to add them, I prefer to 
  put it in the Commit method of UnitOfWork class. Any query could be made with a default filter like 
  "isDeleted == false" which could be added for every entity in the DbContext OnModelCreating with HasFilter(...) 
  method. So that, it will start to add this filter automatically for every query.
* Database is selected as In-Memory Db.
* You can run it on your Visual Studio directly from Web API.
* Default config port of my launch settings is 5188. You can change if you want.
* You can access the API after running the app from http://localhost:{whateverYourPort}/swagger/index.html
