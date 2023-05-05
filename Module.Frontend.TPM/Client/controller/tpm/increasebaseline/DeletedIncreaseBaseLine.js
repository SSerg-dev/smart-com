Ext.define('App.controller.tpm.increasebaseline.DeletedIncreaseBaseLine', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedincreasebaseline directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedincreasebaseline directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedincreasebaseline #datatable': {
                    activate: this.onActivateCard
                },
                'deletedincreasebaseline #detailform': {
                    activate: this.onActivateCard
                },
                'deletedincreasebaseline #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedincreasebaseline #table': {
                    click: this.onTableButtonClick
                },
                'deletedincreasebaseline #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedincreasebaseline #next': {
                    click: this.onNextButtonClick
                },
                'deletedincreasebaseline #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedincreasebaseline #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedincreasebaseline #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedincreasebaseline #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
