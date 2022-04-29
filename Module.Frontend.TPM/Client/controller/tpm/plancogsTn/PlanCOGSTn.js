Ext.define('App.controller.tpm.plancogstn.PlanCOGSTn', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'plancogstn[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'plancogstn directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'plancogstn #datatable': {
                    activate: this.onActivateCard
                },
                'plancogstn #detailform': {
                    activate: this.onActivateCard
                },
                'plancogstn #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'plancogstn #detailform #next': {
                    click: this.onNextButtonClick
                },
                'plancogstn #detail': {
                    click: this.switchToDetailForm
                },
                'plancogstn #table': {
                    click: this.onTableButtonClick
                },
                'plancogstn #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'plancogstn #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'plancogstn #createbutton': {
                    click: this.onCreateButtonClick
                },
                'plancogstn #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'plancogstn #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'plancogstn #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'plancogstn #refresh': {
                    click: this.onRefreshButtonClick
                },
                'plancogstn #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'plancogstn #exportbutton': {
                    click: this.onExportButtonClick
                },
                'plancogstn #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'plancogstn #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'plancogstn #applyimportbutton': {
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
