Ext.define('App.controller.tpm.baseline.DeletedBaseLine', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedbaseline directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedbaseline directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedbaseline #datatable': {
                    activate: this.onActivateCard
                },
                'deletedbaseline #detailform': {
                    activate: this.onActivateCard
                },
                'deletedbaseline #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedbaseline #table': {
                    click: this.onTableButtonClick
                },
                'deletedbaseline #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedbaseline #next': {
                    click: this.onNextButtonClick
                },
                'deletedbaseline #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedbaseline #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedbaseline #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedbaseline #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
