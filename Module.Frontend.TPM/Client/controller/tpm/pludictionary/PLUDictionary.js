Ext.define('App.controller.tpm.pludictionary.PLUDictionary', {
    extend: 'App.controller.core.AssociatedDirectory',
    mixins: ['App.controller.core.ImportExportLogic'],

    init: function () {
        this.listen({
            component: {
                'pludictionary[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'pludictionary': {
                    beforedestroy: this.onPLUDictionaryBeforeDestroy,
                },
                'pludictionary directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'pludictionary #datatable': {
                    activate: this.onActivateCard
                },
                'pludictionary #detailform': {
                    activate: this.onActivateCard
                },
                'pludictionary #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'pludictionary #detailform #next': {
                    click: this.onNextButtonClick
                },
                'pludictionary #detail': {
                    click: this.onDetailButtonClick
                },
                'pludictionary #table': {
                    click: this.onTableButtonClick
                },
                'pludictionary #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'pludictionary #refresh': {
                    click: this.onRefreshButtonClick
                },
                'pludictionary #close': {
                    click: this.onCloseButtonClick
                },
                'pludictionary #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'pludictionary #createbutton': {
                    click: this.onCreateButtonClick
                },
                'pludictionary #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                // import/export
                'pludictionary #exportbutton': {
                    click: this.onExportButtonClick
                },
                'pludictionary #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'pludictionary #loadimportbutton': {
                    click: this.onShowImportFormButtonClick
                },
                'pludictionary #loadimporttemplatebutton': {
                    click: this.onLoadImportTemplateButtonClick
                },
                'pludictionary #detailform': {
                    activate: this.onActivateCard
                },
                'pludictionaryeditor': {
                    afterrender: this.afterrenderWindowEditor,
                },
                'pludictionaryeditor [name=ClientTreeId]': {
                    change: this.onClientTreeIdChange
                }
            }
        });
    },

    afterrenderWindowEditor: function (window, eOpts) {
        var me = this;
        var pludictionaryeditor = Ext.ComponentQuery.query('pludictionaryeditor')[0];

        me.elements = {
            clientTreeId: pludictionaryeditor.down('[name=ClientTreeId]'),
            clientTreeObjectId: pludictionaryeditor.down('[name=ClientTreeObjectId]'),
            pluCode: pludictionaryeditor.down('[name=PluCode]'),
            ean_PC: pludictionaryeditor.down('[name=EAN_PC]'),
        };
    },

    onClientTreeIdChange: function () {
        this.elements.clientTreeObjectId.setValue(this.elements.clientTreeId.getModelData().ClientTreeObjectId);
    },


    onPLUDictionaryBeforeDestroy: function (panel) {
        return true;
    },

});
