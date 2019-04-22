Ext.define('App.controller.tpm.color.DeletedColor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcolor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcolor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcolor #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcolor #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcolor #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcolor #table': {
                    click: this.onTableButtonClick
                },
                'deletedcolor #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcolor #next': {
                    click: this.onNextButtonClick
                },
                'deletedcolor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcolor #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcolor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcolor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
