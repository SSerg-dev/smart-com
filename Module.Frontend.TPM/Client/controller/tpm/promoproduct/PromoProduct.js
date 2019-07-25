Ext.define('App.controller.tpm.promoproduct.PromoProduct', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'promoproduct[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'promoproduct directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'promoproduct #datatable': {
                    activate: this.onActivateCard
                },
                'promoproduct #detailform': {
                    activate: this.onActivateCard
                },
                'promoproduct #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'promoproduct #detailform #next': {
                    click: this.onNextButtonClick
                },
                'promoproduct #detail': {
                    click: this.onDetailButtonClick
                },
                'promoproduct #table': {
                    click: this.onTableButtonClick
                },
                'promoproduct #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'promoproduct #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'promoproduct #createbutton': {
                    click: this.onCreateButtonClick
                },
                'promoproduct #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'promoproduct #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'promoproduct #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'promoproduct #refresh': {
                    click: this.onRefreshButtonClick
                },
                'promoproduct #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'promoproduct #exportbutton': {
                    click: this.onExportButtonClick
                },
                'promoproduct #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'promoproduct #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'promoproduct #loadimporttemplatexlsxbuttonTLC': {
                    click: this.onLoadImportTemplateXLSXTLCButtonClick
                },
                'promoproduct #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onLoadImportTemplateXLSXTLCButtonClick: function (button) {
        var me = this;
        var promoId = button.promoId;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'DownloadTemplateXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                promoId: promoId,
            });

        query = me.buildQuery(query, store)
            .using(Ext.ux.data.BreezeEntityManager.getEntityManager())
            .execute()
            .then(function (data) {
                panel.setLoading(false);
                var filename = data.httpResponse.data.value;
                me.downloadFile('ExportDownload', 'filename', filename);
            })
            .fail(function (data) {
                panel.setLoading(false);
                App.Notify.pushError(me.getErrorMessage(data));
            });
    },
});