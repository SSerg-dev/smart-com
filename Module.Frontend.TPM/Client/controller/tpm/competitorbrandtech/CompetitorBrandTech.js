Ext.define('App.controller.tpm.competitorbrandtech.CompetitorBrandTech', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'competitorbrandtech[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'competitorbrandtech directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'competitorbrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'competitorbrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'competitorbrandtech #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'competitorbrandtech #detailform #next': {
                    click: this.onNextButtonClick
                },
                'competitorbrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'competitorbrandtech #table': {
                    click: this.onTableButtonClick
                },
                'competitorbrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'competitorbrandtech #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'competitorbrandtech #createbutton': {
                    click: this.onCreateButtonClick
                },
                'competitorbrandtech #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'competitorbrandtech #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'competitorbrandtech #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'competitorbrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'competitorbrandtech #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'competitorbrandtech #exportbutton': {
                    click: this.onExportButtonClick
                },
                'competitorbrandtech #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'competitorbrandtech #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'competitorbrandtech #applyimportbutton': {
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
