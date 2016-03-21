$(function() {
    $('#easypiechart-teal').easyPieChart({
        scaleColor: false,
        barColor: '#1ebfae'
    });
	$('#dropdown-menu li:not(:last)').click(function(){
	  var html = $(this).html();
	  $('#dropdownMenuDivider').html(html+'<span class="caret"></span>');
	});
});

$(function() {
    $('#easypiechart-orange').easyPieChart({
        scaleColor: false,
        barColor: '#ffb53e'
    });
});

$(function() {
    $('#easypiechart-red').easyPieChart({
        scaleColor: false,
        barColor: '#f9243f'
    });
});

$(function() {
   $('#easypiechart-blue').easyPieChart({
       scaleColor: false,
       barColor: '#30a5ff'
   });
});

$('#calendar').datepicker({
	});
