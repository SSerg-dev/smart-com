Ext.define('App.controller.tpm.promosupport.DeletedPromoSupport', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedpromosupport directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedpromosupport directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedpromosupport #datatable': {
                    activate: this.onActivateCard
                },
                'deletedpromosupport #detailform': {
                    activate: this.onActivateCard
                },
                'deletedpromosupport #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedpromosupport #table': {
                    click: this.onTableButtonClick
                },
                'deletedpromosupport #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedpromosupport #next': {
                    click: this.onNextButtonClick
                },
                'deletedpromosupport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedpromosupport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedpromosupport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedpromosupport #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});