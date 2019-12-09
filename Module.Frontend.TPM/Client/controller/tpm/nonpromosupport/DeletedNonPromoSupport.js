Ext.define('App.controller.tpm.nonpromosupport.DeletedNonPromoSupport', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletednonpromosupport directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletednonpromosupport directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletednonpromosupport #datatable': {
                    activate: this.onActivateCard
                },
                'deletednonpromosupport #detailform': {
                    activate: this.onActivateCard
                },
                'deletednonpromosupport #detail': {
                    click: this.onDetailButtonClick
                },
                'deletednonpromosupport #table': {
                    click: this.onTableButtonClick
                },
                'deletednonpromosupport #prev': {
                    click: this.onPrevButtonClick
                },
                'deletednonpromosupport #next': {
                    click: this.onNextButtonClick
                },
                'deletednonpromosupport #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletednonpromosupport #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletednonpromosupport #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletednonpromosupport #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});