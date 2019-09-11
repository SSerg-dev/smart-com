Ext.define('App.controller.core.associateduser.aduser.AdUser', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'aduser[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'aduser directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'aduser #datatable': {
                    activate: this.onActivateCard
                },
                'aduser #detailform': {
                    activate: this.onActivateCard
                },
                'aduser #detail': {
                    click: this.switchToDetailForm
                },
                'aduser #table': {
                    click: this.onTableButtonClick
                },
                'aduser #prev': {
                    click: this.onPrevButtonClick
                },
                'aduser #next': {
                    click: this.onNextButtonClick
                },
                'aduser #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'aduser #refresh': {
                    click: this.onRefreshButtonClick
                },
                'aduser #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});