Ext.define('App.controller.core.RecipientSettings', {
    extend: 'Ext.app.Controller',
    init: function () {
        this.listen({
            component: {
                'associatedmailnotificationsettingrecipient editabledetailform field[name=Type]': {
                    change: this.onRecipientTypeChange
                },
                'associatedrecipienteditor field[name=Type]': {
                    change: this.onRecipientTypeChange
                }
            }
        });
    },

    onRecipientTypeChange: function (field, newValue, oldValue) {
        var editorForm = field.up('editabledetailform') ? field.up('editabledetailform') : field.up('editorform'),
            newFieldId = newValue ? ('#' + newValue.toLowerCase() + 'notificationfield') : null,
            oldFieldId = oldValue ? ('#' + oldValue.toLowerCase() + 'notificationfield') : null,
            newField = newFieldId ? editorForm.down(newFieldId) : null,
            oldField = oldFieldId ? editorForm.down(oldFieldId) : null,
            rec = editorForm.items.items[0].getRecord();
        if (newField) {
            newField.show();
            newField.setDisabled(false);
            newField.reset();
            newField.focus(true, 10);
        } if (oldField) {
            oldField.hide();
            oldField.setDisabled(true);
            oldField.reset()
        } else {
            var val = rec.get('Value');
            this.setFieldValue(newField, val);
        }
    },
    setFieldValue: function (field, value) {
        if (field.xtype == "textfield") {
            field.rawValue = value;
        }
        else {
            field.setValue({
                value: value,
                display: value
            });
        }
    }
});