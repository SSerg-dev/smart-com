Ext.define('App.controller.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoactivitydetailsinfo[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoactivitydetailsinfo directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoactivitydetailsinfo #datatable': {
                    activate: this.onActivateCard
                },
                'promoactivitydetailsinfo #detailform': {
                    activate: this.onActivateCard
                },
                'promoactivitydetailsinfo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoactivitydetailsinfo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoactivitydetailsinfo #detail': {
                    click: this.onDetailButtonClick
                },
                'promoactivitydetailsinfo #table': {
                    click: this.onTableButtonClick
                },
                'promoactivitydetailsinfo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoactivitydetailsinfo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoactivitydetailsinfo #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoactivitydetailsinfo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoactivitydetailsinfo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoactivitydetailsinfo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoactivitydetailsinfo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoactivitydetailsinfo #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoactivitydetailsinfo #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoactivitydetailsinfo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoactivitydetailsinfo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoactivitydetailsinfo #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'promoactivitydetailsinfo #customExportXlsxButton': {
                    click: this.onCustomExportXlsxButtonClick
                }
            }
        });
    },

    onCustomExportXlsxButtonClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportXLSX';
        var resource = button.resource || proxy.resourceName;
        var promoactivitydetailsinfo = button.up('promoactivitydetailsinfo');
        var columns = promoactivitydetailsinfo.down('grid').query('gridcolumn[hidden=false]');
        var dataIndexes = '';

        columns.forEach(function (column) {
            dataIndexes += column.dataIndex + ';'
        });

        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                additionalColumn: dataIndexes,
                promoId: promoactivitydetailsinfo.promoId
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                panel.setLoading(false);
                App.Notify.pushInfo('Export task created successfully');
                App.System.openUserTasksPanel()
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    }
});