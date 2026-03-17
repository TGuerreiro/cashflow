import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
  // Configuração para atingir ~50 requisições por segundo
  // 50 RPS * 60 segundos = 3000 requisições totais
  scenarios: {
    constant_request_rate: {
      executor: 'constant-arrival-rate',
      rate: 50,
      timeUnit: '1s',
      duration: '1m',
      preAllocatedVUs: 20,
      maxVUs: 100,
    },
  },
  thresholds: {
    // Requisito do PDF: Máximo 5% de perda (95% de sucesso)
    http_req_failed: ['rate<0.05'], 
    http_req_duration: ['p(95)<500'], // 95% das requisições abaixo de 500ms
  },
};

export default function () {
  const url = 'http://localhost:5001/api/lancamentos';
  const payload = JSON.stringify({
    data: new Date().toISOString().split('T')[0],
    valor: (Math.random() * 1000).toFixed(2),
    tipo: Math.random() > 0.5 ? 1 : 2, // 1: Credito, 2: Debito
    descricao: 'Teste de Stress k6 - Lançamento Automático'
  });

  const params = {
    headers: {
      'Content-Type': 'application/json',
    },
  };

  const res = http.post(url, payload, params);

  check(res, {
    'status is 200 or 201': (r) => r.status === 200 || r.status === 201,
  });

  sleep(0.1);
}
