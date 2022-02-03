Ext.define('App.controller.tpm.competitorpromo.CompetitorPromo', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'competitorpromo[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'competitorpromo directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'competitorpromo #datatable': {
                    activate: this.onActivateCard
                },
                'competitorpromo #detailform': {
                    activate: this.onActivateCard
                },
                'competitorpromo #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'competitorpromo #detailform #next': {
                    click: this.onNextButtonClick
                },
                'competitorpromo #detail': {
                    click: this.onDetailButtonClick
                },
                'competitorpromo #table': {
                    click: this.onTableButtonClick
                },
                'competitorpromo #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'competitorpromo #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'competitorpromo #createbutton': {
                    click: this.onCreateButtonClick
                },
                'competitorpromo #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'competitorpromo #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'competitorpromo #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'competitorpromo #refresh': {
                    click: this.onRefreshButtonClick
                },
                'competitorpromo #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'competitorpromo #exportbutton': {
                    click: this.onExportButtonClick
                },
                'competitorpromo #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'competitorpromo #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'competitorpromo #applyimportbutton': {
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
