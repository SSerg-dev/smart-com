Ext.define('App.controller.tpm.subrange.DeletedSubrange', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedsubrange directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedsubrange directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedsubrange #datatable': {
                    activate: this.onActivateCard
                },
                'deletedsubrange #detailform': {
                    activate: this.onActivateCard
                },
                'deletedsubrange #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedsubrange #table': {
                    click: this.onTableButtonClick
                },
                'deletedsubrange #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedsubrange #next': {
                    click: this.onNextButtonClick
                },
                'deletedsubrange #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedsubrange #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedsubrange #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedsubrange #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
