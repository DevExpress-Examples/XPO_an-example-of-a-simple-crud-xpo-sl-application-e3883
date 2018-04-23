Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text
Imports DevExpress.Xpo.DB
Imports DevExpress.Xpo
Imports SilverlightApplication5

Namespace ApplicationUpdater
	Friend Class Program
		Shared Sub Main(ByVal args() As String)
			Dim cs As String = MSSqlConnectionProvider.GetConnectionString("localhost", "XpoSLExample")
			XpoDefault.DataLayer = XpoDefault.GetDataLayer(cs, AutoCreateOption.DatabaseAndSchema)
			XpoDefault.Session = Nothing
			Try
				Using uow As New UnitOfWork()
					uow.UpdateSchema(GetType(Program).Assembly)
					uow.CreateObjectTypeRecords(GetType(Program).Assembly)
					Dim p As Person = uow.FindObject(Of Person)(Nothing)
					If p Is Nothing Then
                        Dim TempPerson As Person = New Person(uow)
                        TempPerson.Birthdate = New DateTime(1990, 10, 1)
                        TempPerson.LastName = "Smith"
                        TempPerson.FirstName = "Jake"
                        TempPerson = New Person(uow)
                        TempPerson.Birthdate = New DateTime(1982, 2, 20)
                        TempPerson.LastName = "Simpson"
                        TempPerson.FirstName = "Bill"
                        TempPerson = New Person(uow)
                        TempPerson.Birthdate = New DateTime(1972, 5, 17)
                        TempPerson.LastName = "Jonathan"
                        TempPerson.FirstName = "Harry"
                        uow.CommitChanges()
					End If
					Console.WriteLine("Update successful.")
				End Using
			Catch e1 As Exception
				Console.WriteLine("Update failed.")
			End Try
			Console.WriteLine("Press any key...")
			Console.ReadKey()
		End Sub
	End Class
End Namespace