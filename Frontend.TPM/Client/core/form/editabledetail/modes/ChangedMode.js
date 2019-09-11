Ext.define('Core.form.editabledetail.modes.ChangedMode', {

    loadRecord: function (record) {
        console.info('Can not load record while editing');
    },

    getRecord: function () {
        return this.form.getRecord();
    },

    startEditRecord: function () {
        console.info('Record already changed');
    },

    startCreateRecord: function () {
        console.info('Record already created');
    },

    commitChanges: function () {
        console.info('Can not create record while editing');
    },

    rejectChanges: function () {
        this.form.reset(true);
        this.form.loadRecord(this.previousRecord);
        this.setEditing(false);
        this.changeMode(Core.BaseEditableDetailForm.LOADED_MODE);
    }

});