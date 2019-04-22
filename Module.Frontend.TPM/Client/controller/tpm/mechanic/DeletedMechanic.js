Ext.define('App.controller.tpm.mechanic.DeletedMechanic', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedmechanic directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedmechanic directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedmechanic #datatable': {
                    activate: this.onActivateCard
                },
                'deletedmechanic #detailform': {
                    activate: this.onActivateCard
                },
                'deletedmechanic #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedmechanic #table': {
                    click: this.onTableButtonClick
                },
                'deletedmechanic #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedmechanic #next': {
                    click: this.onNextButtonClick
                },
                'deletedmechanic #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedmechanic #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedmechanic #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedmechanic #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
