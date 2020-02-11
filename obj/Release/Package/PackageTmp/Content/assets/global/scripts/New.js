/**
Core script to handle the entire theme and core functions
**/
 $(document).ready(function() {
  $('#country').on('change.states', function() {
    $("#form1").toggle($(this).val() == 'Client');
  }).trigger('change.states');
});