from django.http.response import JsonResponse
from rest_framework.parsers import JSONParser 
from rest_framework import status
 
from fee_app.models import FeesCharged, SegmentFee
from fee_app.serializers import FeeSerializer, FeeUpdateSerializer, ClientSerializer, SegmentSerializer
from rest_framework.decorators import api_view


@api_view(['POST', 'GET'])
def new_segment(request):
    if request.method == 'GET':
        segments = SegmentFee.objects.all()
        serializer = SegmentSerializer(segments, many=True)
        return JsonResponse(serializer.data, safe=False)

    if request.method == 'POST':
        data = JSONParser().parse(request)
        # try:
        #     index = list(map(lambda i: i[1], SEGMENT_OPTIONS)).index(data['name'])
        #     data['name'] = SEGMENT_OPTIONS[index][0]
        # except ValueError as error:
        #     pass
        serializer = SegmentSerializer(data=data)
        if serializer.is_valid():
            serializer.save()
            return JsonResponse(serializer.data, status=status.HTTP_201_CREATED)
        return JsonResponse(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


@api_view(['POST'])
def new_client(request):
    if request.method == 'POST':
        data = JSONParser().parse(request)
        # try:
        #     index = list(map(lambda i: i[1], SEGMENT_OPTIONS)).index(data['segment'])
        #     data['segment'] = SEGMENT_OPTIONS[index][0]
        # except ValueError as error:
        #     pass
        serializer = ClientSerializer(data=data)
        if serializer.is_valid():
            serializer.save()
            return JsonResponse(serializer.data, status=status.HTTP_201_CREATED)
        return JsonResponse(serializer.errors, status=status.HTTP_400_BAD_REQUEST)


@api_view(['POST', 'DELETE', 'GET'])
def fee_list(request):
    if request.method == 'POST':
        charged_data = JSONParser().parse(request)
        charged_serializer = FeeSerializer(data=charged_data)

        if charged_serializer.is_valid():
            charged_serializer.save()
            return JsonResponse(charged_serializer.data, status=status.HTTP_201_CREATED)
        return JsonResponse(charged_serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    elif request.method == 'GET':
        fees_charged = FeesCharged.objects.all()
        
        segment = request.GET.get('segment', None)
        client = request.GET.get('client', None)
        if segment is not None:
            # try:
            #     index = list(map(lambda i: i[1], SEGMENT_OPTIONS)).index(segment)
            #     fees_charged = fees_charged.filter(client__segment__name=SEGMENT_OPTIONS[index][0])
            # except ValueError as error:
            fees_charged = fees_charged.filter(client__segment__name__icontains=segment)
        if client is not None:
            if client.isdigit():
                fees_charged = fees_charged.filter(client__cpf__icontains=client)
            else:
                fees_charged = fees_charged.filter(client__name__icontains=client)
        
        fees_serializer = FeeSerializer(fees_charged, many=True)
        return JsonResponse(fees_serializer.data, safe=False) # 'safe=False' for objects serialization

    elif request.method == 'DELETE':
        count = FeesCharged.objects.all().delete()
        return JsonResponse({'message': '{} Todas entradas deletadas com sucesso!'.format(count[0])}, status=status.HTTP_204_NO_CONTENT)
    return JsonResponse({'message': 'Requisição não encontrada'}, status=status.HTTP_400_BAD_REQUEST)


@api_view(['GET', 'PUT', 'DELETE'])
def fee_detail(request, cpf, id):
    try:
        fees_charged = FeesCharged.objects.get(client__cpf=cpf, id=id)
    except FeesCharged.DoesNotExist:
        return JsonResponse({'message': 'Cliente não encontrado com este CPF.'}, status=status.HTTP_404_NOT_FOUND)

    if request.method == 'GET':
        serializer = FeeSerializer(fees_charged)
        return JsonResponse(serializer.data)

    elif request.method == 'PUT':
        data = JSONParser().parse(request)
        serializer = FeeUpdateSerializer(fees_charged, data=data)
        if serializer.is_valid():
            serializer.save()
            return JsonResponse(serializer.data)
        return JsonResponse(serializer.errors, status=status.HTTP_400_BAD_REQUEST)

    elif request.method == 'DELETE': 
        fees_charged.delete()
        return JsonResponse({'message': 'Deletado com sucesso!'}, status=status.HTTP_204_NO_CONTENT)
