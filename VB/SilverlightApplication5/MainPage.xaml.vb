Imports Microsoft.VisualBasic
Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Net
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Documents
Imports System.Windows.Input
Imports System.Windows.Media
Imports System.Windows.Media.Animation
Imports System.Windows.Shapes
Imports DevExpress.Xpo
Imports System.Threading
Imports DevExpress.Xpo.DB
Imports System.Collections
Imports DevExpress.Xpf.Grid
Imports DevExpress.Xpo.Logger
Imports DevExpress.Data.Filtering.Helpers
Imports DevExpress.Xpo.Metadata
Imports DevExpress.Data.Filtering

Namespace SilverlightApplication5
	Partial Public Class MainPage
		Inherits UserControl
		Private uow As UnitOfWork
		Private personCollection As XPCollection(Of Person)
		Public Sub New()
			InitializeComponent()
			'Initialize connection settings in a separate thread.
				'Create connection to our WCF Service.
				'It is necessary to call UpdataSchema method for all persistent classes.
			ThreadPool.QueueUserWorkItem(Function(o) AnonymousMethod1(o))
		End Sub
		
		Private Function AnonymousMethod1(ByVal o As Object) As Boolean
			XpoDefault.Session = Nothing
			XpoDefault.DataLayer = XpoDefault.GetDataLayer("http://localhost:57861/Service1.svc", AutoCreateOption.SchemaAlreadyExists)
			uow = New UnitOfWork()
			uow.Dictionary.CollectClassInfos(GetType(MainPage).Assembly)
			uow.TypesManager.EnsureIsTypedObjectValid()
			Dispatcher.BeginInvoke(AddressOf BeginInitializeDataSource)
			Return True
		End Function
		Private Sub BeginInitializeDataSource()
			personCollection = New XPCollection(Of Person)(uow)
			personCollection.BindingBehavior = CollectionBindingBehavior.AllowRemove Or CollectionBindingBehavior.AllowNew
			personCollection.DeleteObjectOnRemove = True
			personCollection.LoadAsync(AddressOf EndInitializeDataSource)
		End Sub
		Private Sub EndInitializeDataSource(ByVal result() As ICollection, ByVal ex As Exception)
			'Assign the data source to the control.
			If ex IsNot Nothing Then
				MessageBox.Show(ex.ToString())
			End If
			If result IsNot Nothing Then
				gridControl1.ItemsSource = personCollection
				commitButton.IsEnabled = True
				refreshButton.IsEnabled = True
			Else
				gridControl1.ItemsSource = Nothing
			End If
			gridControl1.IsEnabled = True
		End Sub

		Private Sub commitButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			gridControl1.IsEnabled = False
			commitButton.IsEnabled = False
			refreshButton.IsEnabled = False
			uow.CommitChangesAsync(Function(ex) AnonymousMethod2(ex))
		End Sub
		
		Private Function AnonymousMethod2(ByVal ex As Object) As Boolean
			If ex IsNot Nothing Then
				MessageBox.Show(ex.ToString())
			End If
			gridControl1.IsEnabled = True
			commitButton.IsEnabled = True
			refreshButton.IsEnabled = True
			Return True
		End Function

		Private Sub refreshButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
			gridControl1.IsEnabled = False
			commitButton.IsEnabled = False
			refreshButton.IsEnabled = False
			uow = New UnitOfWork()
			BeginInitializeDataSource()
		End Sub

		Private Sub copyCellDataItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
			Dim menuInfo As GridCellMenuInfo = TryCast(view.GridMenu.MenuInfo, GridCellMenuInfo)
			If menuInfo IsNot Nothing AndAlso menuInfo.Row IsNot Nothing Then
				Dim text As String = "" & gridControl1.GetCellValue(menuInfo.Row.RowHandle.Value, CType(menuInfo.Column, GridColumn)).ToString()
				Clipboard.SetText(text)
			End If
		End Sub

		Private Sub deleteRowItem_ItemClick(ByVal sender As Object, ByVal e As DevExpress.Xpf.Bars.ItemClickEventArgs)
			Dim menuInfo As GridCellMenuInfo = TryCast(view.GridMenu.MenuInfo, GridCellMenuInfo)
			If menuInfo IsNot Nothing AndAlso menuInfo.Row IsNot Nothing Then
				view.DeleteRow(menuInfo.Row.RowHandle.Value)
			End If
		End Sub

		Private Sub gridControl1_CustomUnboundColumnData(ByVal sender As Object, ByVal e As GridColumnDataEventArgs)
			If ReferenceEquals(e.Column, fullNameColumn) AndAlso e.IsGetData Then
                Dim evaluator As New ExpressionEvaluator(uow.GetClassInfo(Of Person)().GetEvaluatorContextDescriptor(), CriteriaOperator.Parse(fullNameColumn.UnboundExpression))
                e.Value = evaluator.Evaluate(gridControl1.GetRowByListIndex(e.ListSourceRowIndex))
            End If
		End Sub
	End Class
End Namespace