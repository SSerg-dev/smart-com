Ext.define('App.controller.tpm.mechanictype.DeletedMechanicType', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedmechanictype directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedmechanictype directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedmechanictype #datatable': {
                    activate: this.onActivateCard
                },
                'deletedmechanictype #detailform': {
                    activate: this.onActivateCard
                },
                'deletedmechanictype #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedmechanictype #table': {
                    click: this.onTableButtonClick
                },
                'deletedmechanictype #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedmechanictype #next': {
                    click: this.onNextButtonClick
                },
                'deletedmechanictype #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedmechanictype #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedmechanictype #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedmechanictype #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
