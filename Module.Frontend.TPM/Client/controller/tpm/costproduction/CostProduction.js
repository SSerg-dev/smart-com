Ext.define('App.controller.tpm.costproduction.CostProduction', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'costproduction[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad
                },
                'costproduction directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'costproduction #datatable': {
                    activate: this.onActivateCard
                },
                'costproduction #detailform': {
                    activate: this.onActivateCard
                },
                'costproduction #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'costproduction #detailform #next': {
                    click: this.onNextButtonClick
                },
                'costproduction #detail': {
                    click: this.onDetailButtonClick
                },
                'costproduction #table': {
                    click: this.onTableButtonClick
                },
                'costproduction #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'costproduction #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'costproduction #createbutton': {
                    click: this.onCreateButtonClick,
                    afterrender: function (button) { button.hide(); }
                },
                
                'costproduction #deletebutton': {
                    click: this.onDeleteButtonClick,
                    afterrender: function (button) { button.hide(); }
                },
                'costproduction #historybutton': {
                    click: this.onHistoryButtonClick,
                },
                'costproduction #refresh': {
                    click: this.onRefreshButtonClick
                },
                'costproduction #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'costproduction #customexportxlsxbutton': {
                    click: this.onExportBtnClick
                },
                'costproduction #exportbutton': {
                    click: this.onExportButtonClick
                },
                'costproduction #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'costproduction #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'costproduction #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
            },
        });
    },

    // переопределение нажатия кнопки экспорта, для определения раздела (TI Cost/Cost Production)
    onExportBtnClick: function (button) {
        var me = this;
        var grid = me.getGridByButton(button);
        var panel = grid.up('combineddirectorypanel');
        var store = grid.getStore();
        var proxy = store.getProxy();
        var actionName = button.action || 'ExportXLSX';
        var resource = button.resource || proxy.resourceName;
        panel.setLoading(true);

        var query = breeze.EntityQuery
            .from(resource)
            .withParameters({
                $actionName: actionName,
                $method: 'POST',
                section: 'costproduction'
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

    // расширенный расширенный фильтр
    onFilterButtonClick: function (button) {
        var grid = this.getGridByButton(button);
        var store = grid.getStore();
        if (store.isExtendedStore) {
            var win = Ext.widget('extmasterfilter', store.getExtendedFilter());
            win.show();
        } else {
            console.error('Extended filter does not implemented for this store');
        }
    },

    onHistoryButtonClick: function (button) {
        if (Ext.ComponentQuery.query('btl')[0]) {
            return;
        }
        var promoSupportController = App.app.getController('tpm.promosupport.PromoSupport');
        promoSupportController.onHistoryButtonClick(button);
    }
});