Ext.define('App.controller.tpm.brand.DeletedBrand', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedbrand directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedbrand directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedbrand #datatable': {
                    activate: this.onActivateCard
                },
                'deletedbrand #detailform': {
                    activate: this.onActivateCard
                },
                'deletedbrand #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedbrand #table': {
                    click: this.onTableButtonClick
                },
                'deletedbrand #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedbrand #next': {
                    click: this.onNextButtonClick
                },
                'deletedbrand #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedbrand #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedbrand #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedbrand #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
