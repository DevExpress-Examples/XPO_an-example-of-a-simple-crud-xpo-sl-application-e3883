Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Runtime.Serialization
Imports System.ServiceModel
Imports System.Text
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo

Namespace SilverlightApplication5.Web
	' NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	Public Class Service1
		Inherits DataStoreService
		Private Shared dataStore As IDataStore
		Shared Sub New()
			Dim cs As String = MSSqlConnectionProvider.GetConnectionString("localhost", "XpoSLExample")
			dataStore = XpoDefault.GetConnectionProvider(cs, AutoCreateOption.SchemaAlreadyExists)
		End Sub
		Public Sub New()
			MyBase.New(dataStore)
		End Sub
	End Class
End Namespace
