Ext.define('App.controller.tpm.tradeinvestment.TradeInvestment', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'tradeinvestment[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'tradeinvestment directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'tradeinvestment #datatable': {
                    activate: this.onActivateCard
                },
                'tradeinvestment #detailform': {
                    activate: this.onActivateCard
                },
                'tradeinvestment #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'tradeinvestment #detailform #next': {
                    click: this.onNextButtonClick
                },
                'tradeinvestment #detail': {
                    click: this.switchToDetailForm
                },
                'tradeinvestment #table': {
                    click: this.onTableButtonClick
                },
                'tradeinvestment #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'tradeinvestment #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'tradeinvestment #createbutton': {
                    click: this.onCreateButtonClick
                },
                'tradeinvestment #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'tradeinvestment #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'tradeinvestment #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'tradeinvestment #refresh': {
                    click: this.onRefreshButtonClick
                },
                'tradeinvestment #close': {
                    click: this.onCloseButtonClick
                },
	            // import/export
                'tradeinvestment #exportbutton': {
                    click: this.onExportButtonClick
                },
                'tradeinvestment #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'tradeinvestment #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'tradeinvestment #applyimportbutton': {
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
            proxy.extraParams.id = this.getRecordId(selModel.getSelection()[0]);

            store.setFixedFilter('HistoricalObjectId', {
                property: '_ObjectId',
                operation: 'Equals',
                value: this.getRecordId(selModel.getSelection()[0])
            });
        }
    }
});
