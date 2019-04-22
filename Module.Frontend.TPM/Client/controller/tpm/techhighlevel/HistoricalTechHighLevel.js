Ext.define('App.controller.tpm.techhighlevel.HistoricalTechHighLevel', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'historicaltechhighlevel directorygrid': {
                    load: this.onGridStoreLoad
                },
                'historicaltechhighlevel directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'historicaltechhighlevel #datatable': {
                    activate: this.onActivateCard
                },
                'historicaltechhighlevel #detailform': {
                    activate: this.onActivateCard
                },
                'historicaltechhighlevel #detail': {
                    click: this.onDetailButtonClick
                },
                'historicaltechhighlevel #table': {
                    click: this.onTableButtonClick
                },
                'historicaltechhighlevel #prev': {
                    click: this.onPrevButtonClick
                },
                'historicaltechhighlevel #next': {
                    click: this.onNextButtonClick
                },
                'historicaltechhighlevel #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'historicaltechhighlevel #refresh': {
                    click: this.onRefreshButtonClick
                },
                'historicaltechhighlevel #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
