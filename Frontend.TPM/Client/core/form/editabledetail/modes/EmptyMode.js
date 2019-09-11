Ext.define('Core.form.editabledetail.modes.EmptyMode', {

    loadRecord: function (record) {
        if (record) {
            this.form.loadRecord(record);
        }
        this.setEditing(false);
        this.changeMode(Core.BaseEditableDetailForm.LOADED_MODE);
    },

    getRecord: function () {
        console.info('Nothing to get');
        return null;
    },

    startEditRecord: function () {
        console.info('Nothing to edit');
    },

    startCreateRecord: function (record) {
        this.form.loadRecord(record || Ext.create(this.model));
        this.changeMode(Core.BaseEditableDetailForm.CREATING_MODE);
        this.setEditing(true);
    },

    commitChanges: function () {
        console.info('Nothing to commit');
    },

    rejectChanges: function () {
        console.info('Nothing to reject');
    }

});