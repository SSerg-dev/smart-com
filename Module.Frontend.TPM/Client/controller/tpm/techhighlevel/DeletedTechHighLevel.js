Ext.define('App.controller.tpm.techhighlevel.DeletedTechHighLevel', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedtechhighlevel directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedtechhighlevel directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedtechhighlevel #datatable': {
                    activate: this.onActivateCard
                },
                'deletedtechhighlevel #detailform': {
                    activate: this.onActivateCard
                },
                'deletedtechhighlevel #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedtechhighlevel #table': {
                    click: this.onTableButtonClick
                },
                'deletedtechhighlevel #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedtechhighlevel #next': {
                    click: this.onNextButtonClick
                },
                'deletedtechhighlevel #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedtechhighlevel #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedtechhighlevel #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedtechhighlevel #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
