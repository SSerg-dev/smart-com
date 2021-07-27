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
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'pludictionary #datatable': {
                    activate: this.onActivateCard
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
                // import/export
                'pludictionary #exportbutton': {
                    click: this.onExportButtonClick
                },
                'pludictionary #detail': {
                    click: this.onDetailButtonClick
                },
                'pludictionary #updatebutton': {
                    click: this.onUpdateButtonClick
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
            }
        });
    },


    onClientTreeIdChange: function () {
        this.elements.clientTreeObjectId.setValue(this.elements.clientTreeId.getModelData().ClientTreeObjectId);
    },


    onPLUDictionaryBeforeDestroy: function (panel) {
        return true;
    },

});
