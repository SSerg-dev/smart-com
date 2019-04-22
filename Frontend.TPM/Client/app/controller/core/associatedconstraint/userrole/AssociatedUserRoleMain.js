Ext.define('App.controller.core.associatedconstraint.userrole.AssociatedUserRoleMain', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                'userrolemain directorygrid': {                 
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'userrolemain[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm,
                },
                'userrolemain #datatable': {
                    activate: this.onActivateCard
                },
                'userrolemain #detailform': {
                    activate: this.onActivateCard
                },
                'userrolemain #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'userrolemain #detailform #next': {
                    click: this.onNextButtonClick
                },
                'userrolemain #detail': {
                    click: this.switchToDetailForm
                },
                'userrolemain #table': {
                    click: this.onTableButtonClick
                },
                'userrolemain #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'userrolemain #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'userrolemain #createbutton': {
                    click: this.onCreateButtonClick
                },
                'userrolemain #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'userrolemain #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'userrolemain #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'userrolemain #refresh': {
                    click: this.onRefreshButtonClick
                },
                'userrolemain #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});