Ext.define('App.controller.tpm.brandtech.DeletedBrandTech', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedbrandtech directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedbrandtech directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedbrandtech #datatable': {
                    activate: this.onActivateCard
                },
                'deletedbrandtech #detailform': {
                    activate: this.onActivateCard
                },
                'deletedbrandtech #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedbrandtech #table': {
                    click: this.onTableButtonClick
                },
                'deletedbrandtech #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedbrandtech #next': {
                    click: this.onNextButtonClick
                },
                'deletedbrandtech #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedbrandtech #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedbrandtech #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedbrandtech #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
