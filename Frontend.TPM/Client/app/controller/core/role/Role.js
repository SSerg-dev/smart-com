Ext.define('App.controller.core.role.Role', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'role[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm
                },
                'role directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'role #datatable': {
                    activate: this.onActivateCard
                },
                'role #detailform': {
                    activate: this.onActivateCard
                },
                'role #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'role #detailform #next': {
                    click: this.onNextButtonClick
                },
                'role #detail': {
                    click: this.switchToDetailForm
                },
                'role #table': {
                    click: this.onTableButtonClick
                },
                'role #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'role #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'role #createbutton': {
                    click: this.onCreateButtonClick
                },
                'role #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'role #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'role #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'role #refresh': {
                    click: this.onRefreshButtonClick
                },
                'role #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    },
});