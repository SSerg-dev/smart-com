Ext.define('App.controller.tpm.plancogsTn.PlanCOGSTn', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'plancogsTn[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'plancogsTn directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'plancogsTn #datatable': {
                    activate: this.onActivateCard
                },
                'plancogsTn #detailform': {
                    activate: this.onActivateCard
                },
                'plancogsTn #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'plancogsTn #detailform #next': {
                    click: this.onNextButtonClick
                },
                'plancogsTn #detail': {
                    click: this.switchToDetailForm
                },
                'plancogsTn #table': {
                    click: this.onTableButtonClick
                },
                'plancogsTn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'plancogsTn #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'plancogsTn #createbutton': {
                    click: this.onCreateButtonClick
                },
                'plancogsTn #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'plancogsTn #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'plancogsTn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'plancogsTn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'plancogsTn #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'plancogsTn #exportbutton': {
                    click: this.onExportButtonClick
                },
                'plancogsTn #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'plancogsTn #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'plancogsTn #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                }
            }
        });
    },

    onHistoryButtonClick: function (button) {
        var grid = this.getGridByButton(button),
            selModel = grid.getSelectionModel();

        if (selModel.hasSelection()) {
            var panel = grid.up('combineddirectorypanel'),
                model = panel.getBaseModel(),
                viewClassName = App.Util.buildViewClassName(panel, model, 'Historical');

            var baseReviewWindow = Ext.widget('basereviewwindow', { items: Ext.create(viewClassName, { baseModel: model }) });
            baseReviewWindow.show();

            var store = baseReviewWindow.down('grid').getStore();
            var proxy = store.getProxy();
            proxy.extraParams.Id = this.getRecordId(selModel.getSelection()[0]);
        }
    }
});
