from django.db import models

from localflavor.br.models import BRCPFField
from . import utils

# SEGMENT_OPTIONS = (
#     ('VA', 'Varejo'),
#     ('PE', 'Personnalite'),
#     ('PR', 'Private'),
# )

class SegmentFee(models.Model):
    # name = models.CharField(choices=SEGMENT_OPTIONS, max_length=2, primary_key=True, error_messages = {"unique": "Segmento com este nome já foi cadastrado."})
    name = models.CharField(max_length=20, primary_key=True, error_messages = {"unique": "Segmento com este nome já foi cadastrado."})
    fee = models.FloatField()

    def __str__(self):
        return self.name

class Client(models.Model):
    # cpf = models.IntegerField(primary_key=True, error_messages = {"unique": "Cliente já cadastrado com este CPF."})
    cpf = BRCPFField(primary_key=True, error_messages = {"unique": "Cliente já cadastrado com este CPF."})
    name = models.TextField()
    segment = models.ForeignKey('SegmentFee', on_delete=models.CASCADE, to_field="name")

    def __str__(self):
        return self.name

class FeesCharged(models.Model):
    client = models.ForeignKey('Client', on_delete=models.CASCADE)
    source_currency_amount = models.FloatField()
    conversion_result = models.FloatField()
    fee = models.FloatField()
    formula = models.CharField(max_length=200, default='Resultado_BRL = (Valor_EUR * Taxa_Cambio) * (1 + Taxa_Segmento)')
    # formula = models.CharField(max_length=200, default='conversion_result = (source_currency_amount * fee) * (1 + fee_segment)')

    def save(self, *args, **kwargs):
        self.fee = utils.get_current_fee()
        self.conversion_result = (self.source_currency_amount * self.fee) * (1 + self.client.segment.fee)
        super(FeesCharged, self).save(*args, **kwargs)

    def __str__(self):
        return self.client.name + str(self.id)