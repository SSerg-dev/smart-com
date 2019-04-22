Ext.define('App.controller.tpm.agegroup.DeletedAgeGroup', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletedagegroup directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletedagegroup directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletedagegroup #datatable': {
                    activate: this.onActivateCard
                },
                'deletedagegroup #detailform': {
                    activate: this.onActivateCard
                },
                'deletedagegroup #detail': {
                    click: this.onDetailButtonClick
                },
                'deletedagegroup #table': {
                    click: this.onTableButtonClick
                },
                'deletedagegroup #prev': {
                    click: this.onPrevButtonClick
                },
                'deletedagegroup #next': {
                    click: this.onNextButtonClick
                },
                'deletedagegroup #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletedagegroup #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletedagegroup #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletedagegroup #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});
