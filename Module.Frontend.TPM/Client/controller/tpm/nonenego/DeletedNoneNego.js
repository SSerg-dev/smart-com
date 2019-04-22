Ext.define('App.controller.tpm.nonenego.DeletedNoneNego', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'deletednonenego directorygrid': {
                    load: this.onGridStoreLoad
                },
                'deletednonenego directorygrid': {
                    itemdblclick: this.onDetailButtonClick,
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'deletednonenego #datatable': {
                    activate: this.onActivateCard
                },
                'deletednonenego #detailform': {
                    activate: this.onActivateCard
                },
                'deletednonenego #detail': {
                    click: this.onDetailButtonClick
                },
                'deletednonenego #table': {
                    click: this.onTableButtonClick
                },
                'deletednonenego #prev': {
                    click: this.onPrevButtonClick
                },
                'deletednonenego #next': {
                    click: this.onNextButtonClick
                },
                'deletednonenego #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'deletednonenego #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'deletednonenego #refresh': {
                    click: this.onRefreshButtonClick
                },
                'deletednonenego #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});