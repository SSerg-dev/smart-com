Ext.define('Core.form.editabledetail.modes.EditingMode', {

    loadRecord: function (record) {
        console.info('Can not load record while editing');
    },

    getRecord: function () {
        console.info('Can not get record while editing');
        return null;
    },

    startEditRecord: function () {
        console.info('Already editing');
    },

    startCreateRecord: function () {
        console.info('Can not start creating record while editing');
    },

    commitChanges: function () {
       
        this.changeMode(Core.BaseEditableDetailForm.CHANGED_MODE);

        return (function (success) {
            if (success) {
                this.form.updateRecord();
                this.setEditing(false);
                this.changeMode(Core.BaseEditableDetailForm.LOADED_MODE);
            } else {
                this.changeMode(Core.BaseEditableDetailForm.EDITING_MODE);
                this.setEditing(true);
            }
        }).bind(this);
    },

    rejectChanges: function () {
        this.form.reset(true);
        this.form.loadRecord(this.previousRecord);
        this.setEditing(false);
        this.changeMode(Core.BaseEditableDetailForm.LOADED_MODE);
    }

});