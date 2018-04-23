using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DevExpress.Xpo;
using System.Threading;
using DevExpress.Xpo.DB;
using System.Collections;
using DevExpress.Xpf.Grid;
using DevExpress.Xpo.Logger;
using DevExpress.Data.Filtering.Helpers;
using DevExpress.Xpo.Metadata;
using DevExpress.Data.Filtering;

namespace SilverlightApplication5 {
    public partial class MainPage : UserControl {
        UnitOfWork uow;
        XPCollection<Person> personCollection;
        public MainPage() {
            InitializeComponent();
            //Initialize connection settings in a separate thread.
            ThreadPool.QueueUserWorkItem(o => {
                //Create connection to our WCF Service.
                XpoDefault.Session = null;
                XpoDefault.DataLayer = XpoDefault.GetDataLayer(
                    "http://localhost:57861/Service1.svc",
                    AutoCreateOption.SchemaAlreadyExists
                 );
                uow = new UnitOfWork();
                //It is necessary to call UpdataSchema method for all persistent classes.
                uow.Dictionary.CollectClassInfos(typeof(MainPage).Assembly);
                uow.TypesManager.EnsureIsTypedObjectValid();
                Dispatcher.BeginInvoke(BeginInitializeDataSource);
            });
        }
        void BeginInitializeDataSource() {
            personCollection = new XPCollection<Person>(uow);
            personCollection.BindingBehavior = CollectionBindingBehavior.AllowRemove | CollectionBindingBehavior.AllowNew;
            personCollection.DeleteObjectOnRemove = true;
            personCollection.LoadAsync(EndInitializeDataSource);
        }
        void EndInitializeDataSource(ICollection[] result, Exception ex) {
            //Assign the data source to the control.
            if (ex != null) {
                MessageBox.Show(ex.ToString());
            }
            if (result != null) {
                gridControl1.ItemsSource = personCollection;
                commitButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
            } else {
                gridControl1.ItemsSource = null;
            }
            gridControl1.IsEnabled = true;
        }

        private void commitButton_Click(object sender, RoutedEventArgs e) {
            gridControl1.IsEnabled = false;
            commitButton.IsEnabled = false;
            refreshButton.IsEnabled = false;
            uow.CommitChangesAsync((ex) => {
                if (ex != null) {
                    MessageBox.Show(ex.ToString());
                }
                gridControl1.IsEnabled = true;
                commitButton.IsEnabled = true;
                refreshButton.IsEnabled = true;
            });
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e) {
            gridControl1.IsEnabled = false;
            commitButton.IsEnabled = false;
            refreshButton.IsEnabled = false;
            uow = new UnitOfWork();
            BeginInitializeDataSource();
        }

        private void copyCellDataItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            GridCellMenuInfo menuInfo = view.GridMenu.MenuInfo as GridCellMenuInfo;
            if (menuInfo != null && menuInfo.Row != null) {
                string text = "" +
                    gridControl1.GetCellValue(menuInfo.Row.RowHandle.Value, (GridColumn)menuInfo.Column).ToString();
                Clipboard.SetText(text);
            }
        }

        private void deleteRowItem_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e) {
            GridCellMenuInfo menuInfo = view.GridMenu.MenuInfo as GridCellMenuInfo;
            if (menuInfo != null && menuInfo.Row != null)
                view.DeleteRow(menuInfo.Row.RowHandle.Value);
        }

        private void gridControl1_CustomUnboundColumnData(object sender, GridColumnDataEventArgs e) {
            if (e.Column == fullNameColumn && e.IsGetData) {
                ExpressionEvaluator evaluator = new ExpressionEvaluator(uow.GetClassInfo<Person>().GetEvaluatorContextDescriptor(), CriteriaOperator.Parse(fullNameColumn.UnboundExpression));
                e.Value = evaluator.Evaluate(gridControl1.GetRowByListIndex(e.ListSourceRowIndex));
            }
        }
    }
}
