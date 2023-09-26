Ext.define('App.controller.core.filesendinterfacesetting.FileSendInterfaceSetting', {
    extend: 'App.controller.core.AssociatedDirectory',

    init: function () {
        this.listen({
            component: {
                'filesendinterfacesetting[isSearch!=true] directorygrid': {
                    load: this.onGridStoreLoad,
                    itemdblclick: this.onDetailButtonClick
                },
                'filesendinterfacesetting directorygrid': {
                    selectionchange: this.onGridSelectionChange,
                    afterrender: this.onGridAfterrender,
                    extfilterchange: this.onExtFilterChange
                },
                'filesendinterfacesetting #datatable': {
                    activate: this.onActivateCard
                },
                'filesendinterfacesetting #detailform': {
                    activate: this.onActivateCard
                },
                'filesendinterfacesetting #detailform #prev': {
                    click: this.onPrevButtonClick
                },
                'filesendinterfacesetting #detailform #next': {
                    click: this.onNextButtonClick
                },
                'filesendinterfacesetting #detail': {
                    click: this.onDetailButtonClick
                },
                'filesendinterfacesetting #table': {
                    click: this.onTableButtonClick
                },
                'filesendinterfacesetting #extfilterbutton': {
                    click: this.onFilterButtonClick
                },
                'filesendinterfacesetting #deletedbutton': {
                    click: this.onDeletedButtonClick
                },
                'filesendinterfacesetting #createbutton': {
                    click: this.onCreateButtonClick
                },
                'filesendinterfacesetting #updatebutton': {
                    click: this.onUpdateButtonClick
                },
                'filesendinterfacesetting #deletebutton': {
                    click: this.onDeleteButtonClick
                },
                'filesendinterfacesetting #historybutton': {
                    click: this.onHistoryButtonClick
                },
                'filesendinterfacesetting #refresh': {
                    click: this.onRefreshButtonClick
                },
                'filesendinterfacesetting #close': {
                    click: this.onCloseButtonClick
                }
            }
        });
    }
});