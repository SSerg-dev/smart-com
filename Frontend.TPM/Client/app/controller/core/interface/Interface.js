Ext.define('App.controller.core.interface.Interface', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'interface[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm
                },
                'interface directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'interface #datatable': {
                    activate: this.onActivateCard
                },
                'interface #detailform': {
                    activate: this.onActivateCard
                },
                'interface #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'interface #detailform #next': {
                    click: this.onNextButtonClick
                },
                'interface #detail': {
                    click: this.switchToDetailForm
                },
                'interface #table': {
                    click: this.onTableButtonClick
                },
                'interface #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'interface #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'interface #createbutton': {
                    click: this.onCreateButtonClick
                },
                'interface #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'interface #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'interface #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'interface #refresh': {
                    click: this.onRefreshButtonClick
                },
                'interface #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});