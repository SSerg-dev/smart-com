Ext.define('App.view.core.common.BooleanCombobox', {
    extend: 'Ext.form.field.ComboBox',
    alias: 'widget.booleancombobox',
    valueField: 'id',
    forceSelection: true,
    editable: false,
    store: {
        type: 'booleanstore'
    },
    listeners: {
        select: function () {
            this.focus(true, 10); // При выборе элемента пропадает фокус. Возвращаем фокус на поле.
        }
    }
});