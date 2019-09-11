Ext.define('App.controller.core.associatedconstraint.constraint.AssociatedConstraint', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                'constraint directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange,
                    itemdblclick: this.switchToDetailForm,
                },
                'historicalconstraint directorygrid': {
                    itemdblclick: this.switchToDetailForm,
                },
                'constraint[isMain=true][isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad
                },
                'constraint #datatable': {
                    activate: this.onActivateCard
                },
                'constraint #detailform': {
                    activate: this.onActivateCard
                },
                'constraint #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'constraint #detailform #next': {
                    click: this.onNextButtonClick
                },
                'constraint #detail': {
                    click: this.switchToDetailForm
                },
                'constraint #table': {
                    click: this.onTableButtonClick
                },
                'constraint #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'constraint #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'constraint #createbutton': {
                    click: this.onCreateButtonClick
                },
                'constraint #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'constraint #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'constraint #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'constraint #refresh': {
                    click: this.onRefreshButtonClick
                },
                'constraint #close': {
                    click: this.onCloseButtonClick
                },
            }
        });
    }

});