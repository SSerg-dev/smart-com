Ext.define('App.controller.tpm.competitor.DeletedCompetitor', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedcompetitor directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedcompetitor directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedcompetitor #datatable': {
                    activate: this.onActivateCard
                },
                'deletedcompetitor #detailform': {
                    activate: this.onActivateCard
                },
                'deletedcompetitor #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedcompetitor #table': {
                    click: this.onTableButtonClick
                },
                'deletedcompetitor #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedcompetitor #next': {
                    click: this.onNextButtonClick
                },
                'deletedcompetitor #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedcompetitor #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedcompetitor #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedcompetitor #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
