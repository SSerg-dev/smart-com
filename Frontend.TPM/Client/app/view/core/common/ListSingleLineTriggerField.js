Ext.define('App.view.core.common.ListSingleLineTriggerField', {
    extend: 'App.view.core.common.SingleLineTriggerField',
    alias: 'widget.listsinglelinetriggerfield',

    separator: ';',

    onTriggerClick: function () {
        var win = Ext.widget(this.widgetName || 'viewtextdatawindow');
        var textField = win.down('field[name=content]');
        if (textField) {
            var value = this.getValue();
            var re = new RegExp(this.separator, 'g');
            value = (value || '').replace(re, '\r\n');
            textField.setValue(value);
            win.show();
        }
    }
});