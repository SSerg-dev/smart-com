Ext.define('App.controller.tpm.competitor.Competitor', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'competitor[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'competitor directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'competitor #datatable': {
                    activate: this.onActivateCard
                },
                'competitor #detailform': {
                    activate: this.onActivateCard
                },
                'competitor #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'competitor #detailform #next': {
                    click: this.onNextButtonClick
                },
                'competitor #detail': {
                    click: this.switchToDetailForm
                },
                'competitor #table': {
                    click: this.onTableButtonClick
                },
                'competitor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'competitor #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'competitor #createbutton': {
                    click: this.onCreateButtonClick
                },
                'competitor #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'competitor #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'competitor #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'competitor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'competitor #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'competitor #exportbutton': {
                    click: this.onExportButtonClick
                },
                'competitor #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'competitor #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'competitor #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
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
