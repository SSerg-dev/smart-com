Ext.define('App.controller.tpm.promoactivitydetailsinfo.PromoActivityDetailsInfoPI', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoactivitydetailsinfopi[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoactivitydetailsinfopi directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoactivitydetailsinfopi #datatable': {
                    activate: this.onActivateCard
                },
                'promoactivitydetailsinfopi #detailform': {
                    activate: this.onActivateCard
                },
                'promoactivitydetailsinfopi #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoactivitydetailsinfopi #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoactivitydetailsinfopi #detail': {
                    click: this.onDetailButtonClick
                },
                'promoactivitydetailsinfopi #table': {
                    click: this.onTableButtonClick
                },
                'promoactivitydetailsinfopi #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoactivitydetailsinfopi #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoactivitydetailsinfopi #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoactivitydetailsinfopi #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoactivitydetailsinfopi #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoactivitydetailsinfopi #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoactivitydetailsinfopi #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoactivitydetailsinfopi #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoactivitydetailsinfopi #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoactivitydetailsinfopi #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoactivitydetailsinfopi #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoactivitydetailsinfopi #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'promoactivitydetailsinfopi #customExportXlsxButton': {
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
        var promoactivitydetailsinfo = button.up('promoactivitydetailsinfopi');
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
                $.connection.sessionHub.server.startMoniringHandler(data.httpResponse.data.value)
                    .done(function () {
                        panel.setLoading(false);
                    })
                    .fail(function (reason) {
                        console.log("SignalR connection failed: " + reason);
                        panel.setLoading(false);
                    });
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    }
});