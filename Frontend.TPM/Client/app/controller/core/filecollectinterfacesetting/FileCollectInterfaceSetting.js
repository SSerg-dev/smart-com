Ext.define('App.controller.core.filecollectinterfacesetting.FileCollectInterfaceSetting', {
    extend: 'App.controller.core.CombinedDirectory',

    init: function () {
        this.listen({
            component: {
                'filecollectinterfacesetting[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.switchToDetailForm
                },
                'filecollectinterfacesetting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'filecollectinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'filecollectinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'filecollectinterfacesetting #detailform #ok': {
                    click: this.onOkButtonClick
                },
                'filecollectinterfacesetting #detailform #cancel': {
                    click: this.onCancelButtonClick
                },
                'filecollectinterfacesetting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'filecollectinterfacesetting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'filecollectinterfacesetting #detail': {
                    click: this.switchToDetailForm
                },
                'filecollectinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'filecollectinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'filecollectinterfacesetting #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'filecollectinterfacesetting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'filecollectinterfacesetting #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'filecollectinterfacesetting #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'filecollectinterfacesetting #historybutton': {
                    click: this.onHistoryButtonClick
                },

                'filecollectinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'filecollectinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});