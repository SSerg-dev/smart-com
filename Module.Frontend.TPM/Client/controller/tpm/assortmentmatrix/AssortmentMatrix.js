Ext.define('App.controller.tpm.assortmentmatrix.AssortmentMatrix', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'assortmentmatrix[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'assortmentmatrix directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'assortmentmatrix #datatable': {
                    activate: this.onActivateCard
                },
                'assortmentmatrix #detailform': {
                    activate: this.onActivateCard
                },
                'assortmentmatrix #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'assortmentmatrix #detailform #next': {
                    click: this.onNextButtonClick
                },
                'assortmentmatrix #detail': {
                    click: this.onDetailButtonClick
                },
                'assortmentmatrix #table': {
                    click: this.onTableButtonClick
                },
                'assortmentmatrix #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'assortmentmatrix #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'assortmentmatrix #createbutton': {
                    click: this.onCreateButtonClick
                },
                'assortmentmatrix #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'assortmentmatrix #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'assortmentmatrix #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'assortmentmatrix #refresh': {
                    click: this.onRefreshButtonClick
                },
                'assortmentmatrix #close': {
                    click: this.onCloseButtonClick
                },
                // import/export
                'assortmentmatrix #exportbutton': {
                    click: this.onExportButtonClick
                },
                'assortmentmatrix #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'assortmentmatrix #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'assortmentmatrix #applyimportbutton': {
                    click: this.onApplyImportButtonClick
                },
                'assortmentmatrixeditor': {
                    afterrender: this.afterrenderWindowEditor,
                },
                'assortmentmatrixeditor [name=ClientTreeId]': {
                    change: this.onClientTreeIdChange
                },
            }
        });
    },

    afterrenderWindowEditor: function (window, eOpts) {
        var me = this;
        var assortmentmatrixeditor = Ext.ComponentQuery.query('assortmentmatrixeditor')[0];

        me.elements = {
            clientTreeId: assortmentmatrixeditor.down('[name=ClientTreeId]'),
            clientTreeObjectId: assortmentmatrixeditor.down('[name=ClientTreeObjectId]'),
        };
    },

    onClientTreeIdChange: function () {
        this.elements.clientTreeObjectId.setValue(this.elements.clientTreeId.getModelData().ClientTreeObjectId);
    },

    onCreateButtonClick: function () {
        this.callParent(arguments);

        var assortmentmatrixeditor = Ext.ComponentQuery.query('assortmentmatrixeditor')[0];
        var createDate = assortmentmatrixeditor.down('[name=CreateDate]');
        createDate.setValue(new Date());
    },
});
