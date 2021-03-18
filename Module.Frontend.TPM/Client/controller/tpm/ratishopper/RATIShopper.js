Ext.define('App.controller.tpm.ratishopper.RATIShopper', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'ratishopper[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'ratishopper directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'ratishopper #datatable': {
                    activate: this.onActivateCard
                },
                'ratishopper #detailform': {
                    activate: this.onActivateCard
                },
                'ratishopper #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'ratishopper #detailform #next': {
                    click: this.onNextButtonClick
                },
                'ratishopper #detail': {
                    click: this.switchToDetailForm
                },
                'ratishopper #table': {
                    click: this.onTableButtonClick
                },
                'ratishopper #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'ratishopper #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'ratishopper #createbutton': {
                    click: this.onCreateButtonClick
                },
                'ratishopper #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'ratishopper #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'ratishopper #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'ratishopper #refresh': {
                    click: this.onRefreshButtonClick
                },
                'ratishopper #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'ratishopper #exportbutton': {
                    click: this.onExportButtonClick
                },
                'ratishopper #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'ratishopper #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'ratishopper #applyimportbutton': {
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
