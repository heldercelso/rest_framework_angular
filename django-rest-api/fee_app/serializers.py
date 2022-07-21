from rest_framework import serializers 
from fee_app.models import FeesCharged, Client, SegmentFee
# from .models import SEGMENT_OPTIONS


class SegmentSerializer(serializers.ModelSerializer):
    
    # name = serializers.CharField(source='get_name_display', read_only=True)
    name = serializers.CharField()
    fee = serializers.FloatField()

    class Meta:
        model = SegmentFee
        fields = ('name',
                #   'name_write',
                  'fee')
        # extra_kwargs = {"name": {"error_messages": {"invalid_choice": "Nome de Segmento escolhido inválido. Opções: " + ", ".join(i[1] + " (" + i[0] + ")" for i in SEGMENT_OPTIONS)}}}
        extra_kwargs = {"name": {"error_messages": {"invalid_choice": "Nome de Segmento escolhido inválido."}}}


class FeeSerializer(serializers.ModelSerializer):

    fee = serializers.ReadOnlyField()
    conversion_result = serializers.ReadOnlyField()
    client_name = serializers.ReadOnlyField(source='client.name')
    # client_segment = serializers.ReadOnlyField(source='client.segment.get_name_display')
    client_segment = serializers.ReadOnlyField(source='client.segment.name')
    segment_fee = serializers.ReadOnlyField(source='client.segment.fee')
    formula = serializers.ReadOnlyField()

    class Meta:
        model = FeesCharged
        fields = ('id',
                  'client',
                  'client_name',
                  'client_segment',
                  'segment_fee',
                  'source_currency_amount',
                  'fee',
                  'conversion_result',
                  'formula')
        extra_kwargs = {"client": {"error_messages": {"does_not_exist": "Cliente não encontrado com este CPF.", "null": "Preencha o campo CPF.", "invalid": "Valor inválido para o campo CPF.", "incorrect_type": "Valor incorreto para CPF."}}, \
                        "source_currency_amount": {"error_messages": {"null": "Preencha o campo CPF.", "invalid": "Valor inválido para o campo quantidade."}}}


class FeeUpdateSerializer(serializers.ModelSerializer):

    fee = serializers.ReadOnlyField()
    conversion_result = serializers.ReadOnlyField()

    class Meta:
        model = FeesCharged
        fields = ('id',
                  'client',
                  'source_currency_amount',
                  'fee',
                  'conversion_result')
        extra_kwargs = {"client": {"error_messages": {"does_not_exist": "Cliente não encontrado com este CPF.", "null": "Preencha o campo CPF.", "invalid": "Valor inválido para o campo CPF."}}, \
                        "source_currency_amount": {"error_messages": {"null": "Preencha o campo CPF.", "invalid": "Valor inválido para o campo Quantidade."}}}


class ClientSerializer(serializers.ModelSerializer):

    class Meta:
        model = Client
        fields = ('cpf',
                  'name',
                  'segment')
        extra_kwargs = {"name": {"error_messages": {"null": "Preencha o campo Nome.", "invalid": "Valor inválido para o campo Nome.", "blank": "Preencha o campo Nome."}}, \
                        "cpf": {"error_messages": {"invalid": "Cliente já cadastrado com este CPF."}}, \
                        "segment": {"error_messages": {"null": "Preencha o campo Segmento.", "invalid": "Valor inválido para o campo Segmento.", "does_not_exist": "Valor inválido para Segmento ou Segmento não cadastrado."}}}
                        # "segment": {"error_messages": {"null": "Preencha o campo Segmento.", "invalid": "Valor inválido para o campo Segmento.", "does_not_exist": "Valor inválido para Segmento ou Segmento não cadastrado. Opções: " + ", ".join(i[1] + " (" + i[0] + ")" for i in SEGMENT_OPTIONS)}}}
